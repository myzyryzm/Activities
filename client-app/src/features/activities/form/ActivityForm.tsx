/** @format */

import React, { useState, FormEvent, useContext, useEffect } from 'react'
import { Segment, Form, Button, Grid } from 'semantic-ui-react'
import { ActivityFormValues } from '../../../app/models/activity'
import { v4 as uuid } from 'uuid'
import ActivityStore from '../../../app/stores/activityStore'
import { observer } from 'mobx-react-lite'
import { RouteComponentProps } from 'react-router'
import { Form as FinalForm, Field } from 'react-final-form'
import TextInput from '../../../app/common/form/TextInput'
import TextAreaInput from '../../../app/common/form/TextAreaInput'
import SelectInput from '../../../app/common/form/SelectInput'
import DateInput from '../../../app/common/form/DateInput'
import { category } from '../../../app/common/options/categoryOptions'
import { combineDateAndTime } from '../../../app/common/util/util'
import {
    combineValidators,
    isRequired,
    composeValidators,
    hasLengthGreaterThan
} from 'revalidate'

const validate = combineValidators({
    title: isRequired({ message: 'The event title is required.' }),
    category: isRequired('Category'),
    description: composeValidators(
        isRequired('Description'),
        hasLengthGreaterThan(4)({
            message: 'Description needs to be at least 5 characters'
        })
    )(),
    city: isRequired('City'),
    venue: isRequired('Venue'),
    date: isRequired('Date'),
    time: isRequired('time')
})

interface DetailParams {
    id: string
}

const ActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({
    match,
    history
}) => {
    const activityStore = useContext(ActivityStore)
    const {
        submitting,
        loadActivity,
        createActivity,
        editActivity
    } = activityStore

    const [activity, setActivity] = useState(new ActivityFormValues())
    const [loading, setLoading] = useState()

    useEffect(() => {
        if (match.params.id) {
            setLoading(true)
            loadActivity(match.params.id)
                .then(activity => setActivity(new ActivityFormValues(activity)))
                .finally(() => setLoading(false))
        }
    }, [loadActivity, match.params.id])

    const handleFinalFormSubmit = (values: any) => {
        const dateAndTime = combineDateAndTime(values.date, values.time)
        //fuck da date and time
        const { date, time, ...activity } = values
        activity.date = dateAndTime
        if (!activity.id) {
            let newActivity = {
                ...activity,
                id: uuid()
            }
            createActivity(newActivity)
        } else {
            editActivity(activity)
        }
    }

    return (
        <Grid>
            <Grid.Column width={10}>
                <Segment clearing>
                    <FinalForm
                        validate={validate}
                        initialValues={activity}
                        onSubmit={handleFinalFormSubmit}
                        render={({ handleSubmit, invalid, pristine }) => (
                            <Form onSubmit={handleSubmit} loading={loading}>
                                <Field
                                    name='title'
                                    placeholder='Title'
                                    value={activity.title}
                                    component={TextInput}
                                />
                                <Field
                                    name='description'
                                    rows={3}
                                    placeholder='Description'
                                    type='textarea'
                                    value={activity.description}
                                    component={TextAreaInput}
                                />
                                <Field
                                    component={SelectInput}
                                    options={category}
                                    name='category'
                                    placeholder='Category'
                                    value={activity.category}
                                />
                                <Form.Group widths='equal'>
                                    <Field
                                        component={DateInput}
                                        name='date'
                                        date={true}
                                        placeholder='Date'
                                        value={activity.date}
                                    />
                                    <Field
                                        component={DateInput}
                                        name='time'
                                        time={true}
                                        placeholder='Time'
                                        value={activity.time}
                                    />
                                </Form.Group>
                                <Field
                                    component={TextInput}
                                    name='city'
                                    placeholder='City'
                                    value={activity.city}
                                />
                                <Field
                                    component={TextInput}
                                    name='venue'
                                    placeholder='Venue'
                                    value={activity.venue}
                                />
                                <Button
                                    loading={submitting}
                                    disabled={loading || invalid || pristine}
                                    floated='right'
                                    positive
                                    type='submit'
                                    content='Submit'
                                />
                                <Button
                                    onClick={() => {
                                        const end = activity.id
                                            ? `/activities/${activity.id}`
                                            : '/activities'
                                        history.push(end)
                                    }}
                                    disabled={loading}
                                    floated='right'
                                    type='button'
                                    content='Cancel'
                                />
                            </Form>
                        )}
                    />
                    {/* <Form onSubmit={handleSubmit}>
            <Form.Input
              onChange={handleInputChange}
              name='title'
              placeholder='Title'
              value={activity.title}
            />
            <Form.TextArea
              onChange={handleInputChange}
              name='description'
              rows={2}
              placeholder='Description'
              value={activity.description}
            />
            <Form.Input
              onChange={handleInputChange}
              name='category'
              placeholder='Category'
              value={activity.category}
            />
            <Form.Input
              onChange={handleInputChange}
              name='date'
              type='datetime-local'
              placeholder='Date'
              value={activity.date}
            />
            <Form.Input
              onChange={handleInputChange}
              name='city'
              placeholder='City'
              value={activity.city}
            />
            <Form.Input
              onChange={handleInputChange}
              name='venue'
              placeholder='Venue'
              value={activity.venue}
            />
            <Button
              loading={submitting}
              floated='right'
              positive
              type='submit'
              content='Submit'
            />
            <Button
              onClick={() => history.push('/activities')}
              floated='right'
              type='button'
              content='Cancel'
            />
          </Form> */}
                </Segment>
            </Grid.Column>
        </Grid>
    )
}

export default observer(ActivityForm)
