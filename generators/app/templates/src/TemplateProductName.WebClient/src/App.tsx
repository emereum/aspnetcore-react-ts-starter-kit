import * as React from "react";
import { BrowserRouter, Link, Route, Switch } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import { Menu } from "semantic-ui-react";
import CreateDinosaurComponent from "./features/dinosaur/CreateDinosaurComponent";
import ErrorBoundaryComponent from "./features/errors/ErrorBoundaryComponent";
import NotFoundComponent from "./features/errors/NotFoundComponent";
import HomeComponent from "./features/home/HomeComponent";
import "react-toastify/dist/ReactToastify.css";

const contentStyle = {
  marginTop: "50px",
  padding: "20px"
};

class App extends React.Component<{}, {}> {
  render() {
    return (
      <BrowserRouter>
        <div className="app">
          <ToastContainer position="top-center" />
          {/* Navigation menu */}
          <Menu className="inverted fixed">
            <Menu.Item as={Link} to="/"><strong>Template Product Name</strong></Menu.Item>
            <Menu.Item as={Link} to="/dinosaur/create">Create Dinosaur</Menu.Item>
          </Menu>

          {/* Component route mappings */}
          {/* See: https://reacttraining.com/react-router/web/example/no-match */}
          <ErrorBoundaryComponent>
            <div style={contentStyle}>
              <Switch>
                <Route path="/" exact={true} component={HomeComponent} />
                <Route path="/dinosaur/create" component={CreateDinosaurComponent} />
                <Route component={NotFoundComponent} />
              </Switch>
            </div>
          </ErrorBoundaryComponent>
        </div>
      </BrowserRouter>
    );
  }
}

export default App;
