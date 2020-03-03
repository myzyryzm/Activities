/** @format */

export interface IProfile {
    displayName: string
    username: string
    bio: string
    image: string
    photos: IPhoto[]
}

export interface IProfileFormValues extends Partial<IProfile> {}

export interface IPhoto {
    id: string
    url: string
    isMain: boolean
}
