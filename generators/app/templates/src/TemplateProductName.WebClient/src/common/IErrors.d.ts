/**
 * Equivalent to Errors provided by the API which contains a set
 * of validation errors keyed by guid then by property name.
 */
interface IErrors {
    [guid: string]: {
        [propertyName: string]: Array<IValidationError>
    };
}

interface IValidationError {
    errorMessage: string;
}