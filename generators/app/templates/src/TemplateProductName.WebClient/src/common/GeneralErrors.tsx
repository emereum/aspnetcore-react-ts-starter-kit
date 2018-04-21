import * as React from "react";
import { Message } from "semantic-ui-react";
import ValidationErrors from "./ValidationErrors";

interface IGeneralErrorsProps {
    errors?: ValidationErrors;
    guid?: string;
}

/**
 * Renders a list of ValidationErrors.
 * 
 * This component is designed to render errors that aren't tied to a specific
 * object or property. It is designed to show general errors. If a guid is
 * specified it will show general errors relating to the object identified by
 * that guid. If no guid is specified it will show errors that aren't tied to
 * any guid (ie. very broad errors).
 * 
 * It is expected that consumers will mount an error component once at the top
 * of a form to capture very broad errors and again for each child object in the
 * form to capture general errors relating to each object. For errors that are
 * tied to individual properties of an object, it is expected that the
 * individual form components (such as inputs and selects) take responsibility
 * for rendering such errors.
 * 
 * N.B: Assumes the errors prop is immutable. Modifications to errors will not
 * cause a re-render.
 */
export default class GeneralErrors extends React.PureComponent<IGeneralErrorsProps, {}> {
    render() {
        const { errors, guid } = this.props;

        if(errors == null) {
            return null;
        }

        const filteredErrors = errors.filter(x => (guid == null || x.guid === guid) && x.property == null);
        
        if(!filteredErrors.length) {
            return null;
        }

        return (
            <Message
                negative={true}
                header="There were some errors with your submission"
                list={filteredErrors.map(x => x.message)}
            />
        );
    }
}