import * as React from "react";
import ValidationErrors from "./ValidationErrors";

interface IPropertyErrorsProps {
    errors?: ValidationErrors;
    errorProperty: string;
    id: string;
    children: JSX.Element;
}

/**
 * Shows a popup around the child component whenever errors exist for the
 * specified id and property name. This should be used in conjunction with
 * GeneralErrors which will show general errors not tied to a specific property.
 * 
 * It is expected that consumers would write a custom input component which
 * wraps itself in this component.
 */
export default class PropertyErrors extends React.Component<IPropertyErrorsProps, {}> {
    render() {
        const { errors, errorProperty, id, children } = this.props;

        const errorPropertyLower = errorProperty.toLowerCase();
        const filteredErrors = (errors || []).filter(x => x.id === id && x.property === errorPropertyLower);

        if(!filteredErrors.length) {
            return children;
        }

        return (
            <div className="property-errors-wrapper">
                <ul>
                    {filteredErrors.map(x => <li>{x.message}</li>)}
                </ul>
                {children}
            </div>
        );
    }
}