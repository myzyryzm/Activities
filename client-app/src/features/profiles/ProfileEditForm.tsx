/** @format */

import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { RootStoreContext } from '../../app/stores/rootStore'
import { combineValidators, isRequired } from 'revalidate'
import { Form as FinalForm, Field } from 'react-final-form'
import { Form, Button } from 'semantic-ui-react'
import TextInput from '../../app/common/form/TextInput'
import TextAreaInput from '../../app/common/form/TextAreaInput'

const validate = combineValidators({
    displayName: isRequired({ message: 'The display name is required.' })
})

const ProfileEditForm = () => {
    const rootStore = useContext(RootStoreContext)
    const { profile, updateProfile } = rootStore.profileStore

    const handleFinalFormSubmit = (values: any) => {
        const nuValues = {
            displayName: values.displayName,
            bio: values.bio,
            username: profile!.username,
            image: profile!.image,
            photos: profile!.photos
        }
        updateProfile(nuValues)
    }

    return (
        <FinalForm
            validate={validate}
            initialValues={{
                displayName: profile!.displayName,
                bio: profile!.bio
            }}
            onSubmit={handleFinalFormSubmit}
            render={({ handleSubmit, invalid, pristine }) => (
                <Form onSubmit={handleSubmit}>
                    <Field
                        name='displayName'
                        placeholder='Display Name'
                        value={profile!.displayName}
                        component={TextInput}
                    />
                    <Field
                        name='bio'
                        rows={3}
                        placeholder='Bio'
                        type='textarea'
                        value={profile!.bio}
                        component={TextAreaInput}
                    />
                    <Button
                        disabled={invalid || pristine}
                        floated='right'
                        positive
                        type='submit'
                        content='Submit'
                    />
                </Form>
            )}
        ></FinalForm>
    )
}

export default observer(ProfileEditForm)
