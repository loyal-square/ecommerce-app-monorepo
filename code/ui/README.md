# Ui

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 16.1.2.

The following steps were taken to create the Angular workspace.

```bash
# Create workspace and project
ng new {workspace name} --create-application false
cd {workspace}
ng g application {project name} --routing --style=css
cd {project}
ionic init {project name} --type=angular --project-id={project name}

# Create library
ng g lib ngx-{library name}

# To build and watch library
ng build ngx-{library name} --watch

# To run the application
ng serve {project name}
```

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.
