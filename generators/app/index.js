"use strict";

var Generator = require("yeoman-generator");
var rename = require("gulp-rename");
var replace = require("gulp-replace");

// See: https://github.com/yeoman/generator-generator/blob/master/subgenerator/templates/index.js
module.exports = class extends Generator {
    constructor(args, opts) {
        super(args, opts);
        
        // Values in the template project which should be
        // substituted with user-specified values.
        this.substitutions = [
            {
                templateValue: "Template Product Name",
                name: "productName",
                message: "Product Name in title case (e.g. ACME Widgetizer)",
                default: this.appname // Default to current folder name
            },
            {
                templateValue: "Template Author Name",
                name: "authorName",
                message: "Author Name in title case (e.g. ACME Incorporated)"
            }
        ];
        
        // Describes several transformations of the template
        // values such that we can substitute out different
        // formattings of the template values (such as PascalCase
        // or kebab-case).
        this.mappers = [
            // Title Case
            function(x) { return x; },
            
            // PascalCase
            function(x) { return x.replace(/[^a-zA-Z0-9]/g, ""); },
            
            // lowercase
            function(x) { return x.replace(/[^a-zA-Z0-9]/g, "").toLowerCase(); },
            
            // kebab-case
            function(x) { return x.replace(/ /g, "-").replace(/[^-a-zA-Z0-9]/, "").toLowerCase(); }
        ];
    }
    
    prompting() {
        // See: http://yeoman.io/authoring/user-interactions.html
        var prompts = this.substitutions.map(function(x) {
            return {
                type: "input",
                name: x.name,
                message: x.message,
                default: x.default
            };
        });
        
        return this.prompt(prompts)
            .then(function(props) {
                this.props = props;
            }.bind(this))
    }
    
    writing() {
        this._setupTransformers();
        this._copyTemplateToOutputFolder();
    }
    
    _setupTransformers() {
        // Set up stream transformers which perform the mapping of the input
        // files to output files tailored to the template values specified
        // by the user.
        this.substitutions.forEach(function(substitution) {
            this.mappers.forEach(function(mapper) {
                var findValue = mapper(substitution.templateValue);
                var findRegex = new RegExp(findValue, "g");
                var replaceValue = mapper(this.props[substitution.name]);
                
                this.registerTransformStream(rename(function(path) {
                    // Map file names
                    path.basename = path.basename.replace(findRegex, replaceValue);
                    
                    // Map directory names
                    path.dirname = path.dirname.replace(findRegex, replaceValue);
                }));
                
                // Map file contents
                this.registerTransformStream(replace(findRegex, replaceValue));
            }.bind(this));
        }.bind(this));
        
        // Rename .npmignore to .gitignore
         this.registerTransformStream(rename(function(path) {
            path.basename = path.basename.replace(".npmignore", ".gitignore");
        }));
    }
    
    _copyTemplateToOutputFolder() {
        // Copy the entire template folder to the output folder
        this.fs.copy(
            [
                this.templatePath(),
                // If we're working on the template, we don't want to copy
                // any build artifacts that are currently in the template
                // directory. This is functionally equivalent to applying
                // the .gitignore files to the yeoman copying process.
                // See: https://github.com/gulpjs/gulp/issues/165
                "!**/packages",
                "!**/packages/**",
                "!**/.vs",
                "!**/.vs/**",
                "!**/bin",
                "!**/bin/**",
                "!**/obj",
                "!**/obj/**",
                "!**/*.suo",
                "!**/*.user",
                "!**/*.lock.json",
                "!**/TestResult.xml",
                "!**/*.nupkg",
                "!**/*.local.json",
                
                "!**/node_modules",
                // "!**/node_modules/**", // Causes strange file copy errors
                "!**/coverage",
                "!**/coverage/**",
                "!**/build",
                "!**/build/**",
                "!**/npm-debug.log*"
            ],
            this.destinationPath(),
            { globOptions: { dot: true } } // Copy dotfiles
        );
    }
}