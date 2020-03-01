/** @format */
import { RootStore } from './rootStore'
import { observable, action } from 'mobx'
export default class ModalStore {
    rootStore: RootStore
    constructor(rootStore: RootStore) {
        this.rootStore = rootStore
    }
    //shallow converts structure to 1 layer structure so body will be observed 1 level deep (b/c we passing in a react component as a body)
    @observable.shallow modal = {
        open: false,
        body: null
    }

    @action openModal = (content: any) => {
        this.modal.open = true
        this.modal.body = content
    }

    @action closeModal = () => {
        this.modal.open = false
        this.modal.body = null
    }
}
