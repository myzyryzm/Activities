import React from 'react'
import { ICar } from './demo'

interface IProps {
    car: ICar
}
//React.FC takes in a generic type parameter which defines what type of properties we are going to receive
const CarItem: React.FC<IProps> = ({car}) => {
    return (
        <div>
            <h1>{car.color}</h1>
        </div>
    )
}

export default CarItem
