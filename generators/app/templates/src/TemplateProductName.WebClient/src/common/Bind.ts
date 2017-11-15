import * as R from "ramda";

/**
 * Provides data-binding like functionality to any component which implements
 * value and onChange. Example usage:
 * 
 * <SomeInput {...Bind(person, "name")} />
 * 
 * @param context The object whose property should be shown.
 * @param property The property of the object to show.
 */
function Bind(
    context: any,
    property: string,
    onChange?: (e: any, data: any) => void,
    errors?: IErrors,
    errorProperty?: string) {
    if(!(property in context)) {
        throw "Bound property does not exist: " + property;
    }

    // If the context has a guid and we have a dictionary of
    // guids -> properties -> errors then check whether this property is in error
    // and provide that information to the bound component.
    //
    // Consumers can default to checking the bound property name or override
    // and use a different name for error checking. This is useful when the model
    // submitted to the API doesn't exactly match the model being presented (e.g.
    // a property might be called "thing" in a component but submitted to the API
    // as "thingCode", in such cases the errorProperty should be set to "thingCode").
    const checkErrorProperty = (errorProperty != null ? errorProperty : property).toLowerCase();
    const hasErrors =
        context.guid != null &&
        R.path([context.guid, checkErrorProperty], errors) != null;

    return {
        value: context[property],
        onChange: (e: any, data: any) =>  {
            context[property] = data.value;
            if(onChange != null) {
                onChange(e, data);
            }
        },
        error: hasErrors
    };
}

export default Bind;