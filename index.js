const prompts = require("prompts");
const gulp = require("gulp")
const rename = require("gulp-rename");
const replace = require("gulp-replace");

(async () => {
    // Values in the template project which should be substituted with
    // user-specified values.
    const substitutions = [
        {
            name: "productName",
            message: "What is the product name (in title case, e.g. ACME Widget)?",            
            templateValue: "Template Product Name"
        },
        {
            name: "authorName",
            message: "What is the author name (in title case, e.g. ACME Incorporated)?",
            templateValue: "Template Author Name"
        }
    ];

    const response = await prompts(substitutions.map(x => ({
        name: x.name,
        message: x.message,
        type: "text",
        validate: x => x.length > 0
    })));

    const completedSubstitutions = substitutions.map(x => ({...x, value: response[x.name]}));
        
    // Describes several transformations of the template values so we can
    // substitute out different formattings of the template values (such as
    // PascalCase or kebab-case).
    const mappers = [
        // Title Case
        x => x,
        // PascalCase
        x => x.replace(/[^a-zA-Z0-9]/g, ""),
        // lowercase
        x => x.replace(/[^a-zA-Z0-9]/g, "").toLowerCase(),
        // kebab-case
        x => x.replace(/ /g, "-").replace(/[^-a-zA-Z0-9]/, "").toLowerCase(),
        // snake_case
        x => x.replace(/ /g, "_").replace(/[^_a-zA-Z0-9]/, "").toLowerCase()
    ];

    // Map template file names and content to output
    mappers
        .flatMap(mapper => completedSubstitutions.map(sub => ({
            substitution: sub,
            mapper: mapper
        })))
        .reduce((pipeline, x) => {
            const { substitution, mapper } = x;
            const findRegex = new RegExp(mapper(substitution.templateValue), "g");
            const replaceValue = mapper(substitution.value);

            return pipeline
                .pipe(rename(path => {
                    // Map file names
                    path.basename = path.basename.replace(findRegex, replaceValue);
                    
                    // Map directory names
                    path.dirname = path.dirname.replace(findRegex, replaceValue);
                }))
                // Map file contents
                .pipe(replace(findRegex, replaceValue));
        },
        gulp.src(__dirname + "/template/**/*"))
        .pipe(gulp.dest("."));
})();