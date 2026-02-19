import React from "react";
import ReactDOM from "react-dom/client";
import { Auth0Provider } from "@auth0/auth0-react";
import App from "./App";
import "./index.css";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <Auth0Provider
      domain="dev-u7el5z8wrntiommw.eu.auth0.com"
      clientId="cw8yHR5E9GemYe3QgnTHyoVwBBKFDYZT"
      authorizationParams={{
        redirect_uri: window.location.origin,
        audience: "https://company-api",
        scope: "openid profile email offline_access"
      }}
      cacheLocation="localstorage"
      useRefreshTokens={true}
    >
      <App />
    </Auth0Provider>
  </React.StrictMode>
);
