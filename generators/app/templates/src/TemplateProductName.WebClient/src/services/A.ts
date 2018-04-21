// These methods are used for asserting that a value is of a certain type.
// This is important when reading in JSON data and asserting that the shape
// of the JSON object matches out expectations

export function nullableBoolean(val: any): boolean | undefined {
    if(val == null) { return undefined; }
    if(typeof val === "boolean") { return val; }
    throw new Error("Type mismatch");
}

export function nullableNumber(val: any): number | undefined {
    if(val == null) { return undefined; }
    if(typeof val === "number") { return val; }
    throw new Error("Type mismatch");
}

export function nullableString(val: any): string | undefined {
    if(val == null) { return undefined; }
    if(typeof val === "string") { return val; }
    throw new Error("Type mismatch");
}

export function nullableList(val: any): any[] | undefined {
    if(val == null) { return undefined; }
    if(Array.isArray(val)) { return val; }
    throw new Error("Type mismatch");
}

export function boolean(val: any): boolean {
    if(val == null) { throw new Error("Unexpected null value"); }
    if(typeof val === "boolean") { return val; }
    throw new Error("Type mismatch");
}

export function number(val: any): number {
    if(val == null) { throw new Error("Unexpected null value"); }
    if(typeof val === "number") { return val; }
    throw new Error("Type mismatch");
}

export function string(val: any): string {
    if(val == null) { throw new Error("Unexpected null value"); }
    if(typeof val === "string") { return val; }
    throw new Error("Type mismatch");
}

export function list(val: any): any[] {
    if(val == null) { throw new Error("Unexpected null value"); }
    if(Array.isArray(val)) { return val; }
    throw new Error("Type mismatch");
}