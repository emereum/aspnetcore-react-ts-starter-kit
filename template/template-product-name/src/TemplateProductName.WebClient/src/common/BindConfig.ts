import Bind from "./Bind";
import ValidationErrors from "./ValidationErrors";

interface IBindConfigOptions<TContext> {
    context: TContext;
    errors?: ValidationErrors;
}

/**
 * Partially applies a handful of binding options to Bind. These options are
 * usually constant for a given component so this saves us having to specify
 * them for every binding used in a component.
 */
function BindConfig<TContext>(options: IBindConfigOptions<TContext>) {
    return (property: keyof TContext, onChange?: (e: React.SyntheticEvent<any>, data?: { value: any }) => void, errorProperty?: string) =>
        Bind(options.context, property, onChange, options.errors, errorProperty);
}

export default BindConfig;