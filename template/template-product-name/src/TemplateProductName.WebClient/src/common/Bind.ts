import { action } from "mobx";
import ValidationErrors from "./ValidationErrors";

/**
 * Provides data-binding like functionality to any component which implements
 * value and onChange. Example usage:
 * 
 * <SomeInput {...Bind(person, "name")} />
 * 
 * See: https://blog.mariusschulz.com/2017/01/06/typescript-2-1-keyof-and-lookup-types
 * 
 * @param context The object whose property should be shown.
 * @param property The property of the object to show.
 */
function Bind<TContext, TProp extends keyof TContext>(
    context: TContext,
    property: TProp,
    onChange?: (e: React.SyntheticEvent<any>, data?: { value: any }) => void,
    errors?: ValidationErrors,
    errorProperty?: string) {
    if(!(property in context)) {
        throw new Error("Bound property does not exist: " + property);
    }
    return {
        value: context[property],
        onChange: action((...args: any[]) =>  {
            if(args.length === 1) {           
                // We're bound to a standard react component
                context[property] = (args[0] as React.ChangeEvent<any>).target.value;
            } else if(args.length === 2) {
                // We're bound to a semantic-ui component
                context[property] = args[1].value;
            } else {
                throw new Error(
                      "Unexpected number of arguments to onChange callback. " +
                      "Did you bind to something other than a React input or " +
                      "semantic-ui component?");
            }

            if(onChange != null) {
                onChange.apply(null, <any>args);
            }
        }),
        errors,
        errorProperty: errorProperty || property
    };
}

export default Bind;