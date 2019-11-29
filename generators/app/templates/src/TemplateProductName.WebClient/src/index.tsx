import * as React from "react";
import * as ReactDOM from "react-dom";
import App from "./App";
import registerServiceWorker from "./registerServiceWorker";
import "semantic-ui-css/semantic.min.css";
import "./index.css";

ReactDOM.render(
  <App />,
  document.getElementById("root") as HTMLElement
);
// this caches our index.html which isn't what we want right now. in lieu of understanding
// this we are going to comment it out.
//registerServiceWorker();

// Unregister service workers till we understand this better.
// Causes too much caching.
navigator.serviceWorker.getRegistrations().then(

  function(registrations) {

      for(let registration of registrations) {  
          registration.unregister();

      }

});

