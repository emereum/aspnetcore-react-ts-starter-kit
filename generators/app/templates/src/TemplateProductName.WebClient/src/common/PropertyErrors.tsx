import * as React from "react";
import { Popup } from "semantic-ui-react";
import ValidationErrors from "./ValidationErrors";

interface IPropertyErrorsProps {
    errors?: ValidationErrors;
    errorProperty: string;
    guid: string;
    children: JSX.Element;
}

/**
 * Shows a popup around the child component whenever errors exist for the
 * specified guid and property name. This should be used in conjunction with
 * GeneralErrors which will show general errors not tied to a specific property.
 * 
 * It is expected that consumers would write a custom input component which
 * wraps itself in this component.
 */
export default class PropertyErrors extends React.Component<IPropertyErrorsProps, {}> {
    render() {
        const { errors, errorProperty, guid, children } = this.props;

        const errorPropertyLower = errorProperty.toLowerCase();
        var filteredErrors = (errors || []).filter(x => x.guid === guid && x.property === errorPropertyLower);

        if(!filteredErrors.length) {
            return children;
        }

        console.log(filteredErrors);

        return (
            <Popup
                trigger={<div className="property-errors-wrapper">{children}</div>}
                content={filteredErrors.map(x => x.message)}
                position="right center"
            />
        );
    }
}