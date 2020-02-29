/** @format */

export interface IActivity {
    id: string
    title: string
    description: string
    category: string
    date: Date
    city: string
    venue: string
}
//adding partial in front of IActivity allows all the properties passed in to be optional
export interface IActivityFormValues extends Partial<IActivity> {
    time?: Date
}

export class ActivityFormValues implements IActivityFormValues {
    id?: string = undefined
    title: string = ''
    category: string = ''
    description: string = ''
    date?: Date = undefined
    city: string = ''
    venue: string = ''
    time?: Date = undefined

    constructor(init?: IActivityFormValues) {
        if (init && init.date) {
            init.time = init.date
        }
        Object.assign(this, init)
    }
}
