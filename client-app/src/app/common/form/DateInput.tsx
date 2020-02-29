/** @format */

import React from 'react'
import { FieldRenderProps } from 'react-final-form'
import { FormFieldProps, Form, Label } from 'semantic-ui-react'
import { DateTimePicker } from 'react-widgets'

interface IProps
    extends FieldRenderProps<Date, HTMLInputElement>,
        FormFieldProps {}

const DateInput: React.FC<IProps> = ({
    input,
    width,
    placeholder,
    date = false,
    time = false,
    meta: { touched, error },
    ...rest
}) => {
    return (
        <Form.Field error={touched && !!error} width={width}>
            <DateTimePicker
                placeholder={placeholder}
                value={input.value || null}
                onBlur={input.onBlur}
                onChange={input.onChange}
                onKeyPress={e => e.preventDefault()}
                date={date}
                time={time}
            />
            {touched && error && (
                <Label basic color='red'>
                    {error}
                </Label>
            )}
        </Form.Field>
    )
}

export default DateInput
