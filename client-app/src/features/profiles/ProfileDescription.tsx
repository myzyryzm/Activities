/** @format */

import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import { Tab, Grid, Header } from 'semantic-ui-react'
import { RootStoreContext } from '../../app/stores/rootStore'
import ProfileEditForm from './ProfileEditForm'

const ProfileDescription = () => {
    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16} style={{ paddingBottom: 0 }}>
                    <Header floated='left' icon='user' content='About' />
                </Grid.Column>
                <Grid.Column width={16}>
                    <ProfileEditForm />
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
}

export default observer(ProfileDescription)
