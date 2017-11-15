import * as React from "react";
import { Menu } from "semantic-ui-react";
const { BrowserRouter, Link, Route, Switch } = require("react-router-dom"); // react-router has no typings so "require" is used.
import HomeComponent from "./features/home/HomeComponent";
import CreateDinosaurComponent from "./features/dinosaur/CreateDinosaurComponent";
import NotFoundComponent from "./features/errors/NotFoundComponent";

class App extends React.Component<{}, {}> {
  render() {
    return (
      <BrowserRouter>
        <div className="app">
          {/* Navigation menu */}
          <Menu className="inverted fixed">
            <Menu.Item as={Link} to="/"><strong>Template Product Name</strong></Menu.Item>
            <Menu.Item as={Link} to="/dinosaur/create">Create Dinosaur</Menu.Item>
          </Menu>

          {/* Component route mappings */}
          {/* See: https://reacttraining.com/react-router/web/example/no-match */}
          <div className="content">
            <Switch>
              <Route path="/" exact={true} component={HomeComponent} />
              <Route path="/dinosaur/create" component={CreateDinosaurComponent} />
              <Route component={NotFoundComponent} />
            </Switch>
          </div>
        </div>
      </BrowserRouter>
    );
  }
}

export default App;
