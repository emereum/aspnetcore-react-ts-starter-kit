import * as React from "react";
import ValidationErrors from "./ValidationErrors";

interface IInputProps {
    label: string;
    value: string;
    onChange: any;
    errors?: ValidationErrors;
    errorProperty?: string;
    id: string;
    type?: undefined | "password";
}

export default class TextInput extends React.Component<IInputProps, {}> {
    render() {
        const { errors, errorProperty, id, label, value, onChange, type } = this.props;

        const errorPropertyLower = (errorProperty || "notaprop").toLowerCase();
        const filteredErrors = (errors || []).filter(x => x.id === id && x.property === errorPropertyLower);

        if(!filteredErrors.length) {
            return (
                <div className="field">
                    <label className="label">{label}</label>
                    <div className="control">
                        <input className="input" type={type || "text"} value={value} onChange={onChange} />
                    </div>
                </div>
            )
        }

        return (
            <div className="field">
                <label className="label">{label}</label>
                <div className="control has-icons-right">
                    <input className="input is-danger" type={type || "text"} value={value} onChange={onChange} />
                    <span className="icon is-small is-right">
                    <i className="fas fa-exclamation-triangle"></i>
                    </span>
                </div>
                {filteredErrors.map(x => <p className="help is-danger">{x.message}</p>)}
            </div>       
        );
    }
}




