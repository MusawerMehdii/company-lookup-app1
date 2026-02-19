import { useAuth0 } from "@auth0/auth0-react";
import CompanySearch from "./CompanySearch";

function App() {
  const {
    loginWithRedirect,
    logout,
    isAuthenticated,
    isLoading,
    user
  } = useAuth0();

  if (isLoading) {
    return <div className="center">Loading...</div>;
  }

  if (!isAuthenticated) {
    return (
      <div className="center">
        <h1>Clean Company API</h1>
        <button className="btn" onClick={() => loginWithRedirect()}>
          Login
        </button>
      </div>
    );
  }

  return (
    <div className="app-container">
      <div className="header">
        <h1>Company Dashboard</h1>

        <div>
          <span className="muted">{user?.email}</span>
          <button
            className="btn logout"
            onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })}
          >
            Logout
          </button>
        </div>
      </div>

      <CompanySearch />
    </div>
  );
}

export default App;
