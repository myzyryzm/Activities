/** @format */

import { RootStore } from './rootStore'
import { observable, action, reaction } from 'mobx'

export default class CommonStore {
    rootStore: RootStore
    constructor(rootStore: RootStore) {
        this.rootStore = rootStore
        //reaction runs when this.token changes; then the function after the variable u declare as reacting will run
        reaction(
            () => this.token,
            token => {
                if (token) {
                    window.localStorage.setItem('jwt', token)
                } else {
                    window.localStorage.removeItem('jwt')
                }
            }
        )
    }

    @observable token: string | null = window.localStorage.getItem('jwt')
    @observable appLoaded = false

    @action setToken = (token: string | null) => {
        this.token = token
    }

    @action setAppLoaded = () => {
        this.appLoaded = true
    }
}
