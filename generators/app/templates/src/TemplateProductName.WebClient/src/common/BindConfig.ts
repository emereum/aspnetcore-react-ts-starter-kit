import Bind from "./Bind";

interface IBindConfigOptions {
    context: any;
    errors?: IErrors;
}

/**
 * Partially applies a handful of binding options to Bind. These options are
 * usually constant for a given component so this saves us having to specify
 * them for every binding used in a component.
 */
function BindConfig(options: IBindConfigOptions) {
    return (property: string, onChange?: (e: any, data: any) => void, errorProperty?: string) =>
        Bind(options.context, property, onChange, options.errors, errorProperty);
}

export default BindConfig;