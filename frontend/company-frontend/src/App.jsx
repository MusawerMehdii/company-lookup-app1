import { useAuth0 } from "@auth0/auth0-react";
import CompanySearch from "./company-search";
import "./index.css";

function App() {
  const { loginWithRedirect, logout, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <div className="center">Loading…</div>;
  }

  return (
    <div className="app-container">
      <header className="header">
        <h1>Company Lookup</h1>

        {isAuthenticated ? (
          <button
            className="btn"
            onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })}
          >
            Logout
          </button>
        ) : (
          <button className="btn" onClick={loginWithRedirect}>
            Login
          </button>
        )}
      </header>

      {isAuthenticated && <CompanySearch />}
    </div>
  );
}

export default App;
