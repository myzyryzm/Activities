/** @format */

import React, { useContext, useEffect } from 'react'
import { Grid, Header } from 'semantic-ui-react'
import ActivityStore from '../../../app/stores/activityStore'
import { observer } from 'mobx-react-lite'
import { RouteComponentProps } from 'react-router'
import LoadingComponent from '../../../app/layout/LoadingComponent'
import ActivityDetailedHeader from './ActivityDetailedHeader'
import ActivityDetailedInfo from './ActivityDetailedInfo'
import ActivityDetailedChat from './ActivityDetailedChat'
import ActivityDetailedSidebar from './ActivityDetailedSidebar'

//need to specify that id is a string so match.params.id is a string
//pass it is as a type to the RouteComponentProps
interface DetailParams {
    id: string
}

const ActivityDetails: React.FC<RouteComponentProps<DetailParams>> = ({
    match,
    history
}) => {
    const activityStore = useContext(ActivityStore)
    const { activity, loadActivity, loadingInitial } = activityStore

    useEffect(() => {
        //.id refers to :id in path of route
        loadActivity(match.params.id)
    }, [loadActivity, match.params.id, history])

    if (loadingInitial)
        return <LoadingComponent content='Loading activity...' />

    if (!activity) return <Header>Fuck Off</Header>

    return (
        <Grid>
            <Grid.Column width={10}>
                <ActivityDetailedHeader activity={activity} />
                <ActivityDetailedInfo activity={activity} />
                <ActivityDetailedChat />
            </Grid.Column>
            <Grid.Column width={6}>
                <ActivityDetailedSidebar />
            </Grid.Column>
        </Grid>
    )
}

export default observer(ActivityDetails)
