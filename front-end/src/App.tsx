import { useAuth0 } from "@auth0/auth0-react";
import { useState } from "react";
import { searchCompanies, getCompany } from "./services/api";

type Company = {
  organizationNumber: string;
  name: string;
  organizationForm: string;
  municipality: string;
};

function App() {
  const {
    loginWithRedirect,
    logout,
    user,
    isAuthenticated,
    getAccessTokenSilently,
  } = useAuth0();

  const [query, setQuery] = useState("");
  const [companies, setCompanies] = useState<Company[]>([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const isOrgNumberSearch = (value: string) => /^\d+$/.test(value);

  const handleSearch = async () => {
    if (!query.trim()) {
      setError("Please enter a search value.");
      return;
    }

    try {
      setLoading(true);
      setError("");

      const token = await getAccessTokenSilently();

      let results: Company[] = [];

      if (isOrgNumberSearch(query)) {
        const company = await getCompany(query, token);
        if (company) results = [company];
      } else {
        results = await searchCompanies(query, token);
      }

      if (!results || results.length === 0) {
        setError("No results found.");
      }

      setCompanies(results);
    } catch {
      setError("Something went wrong.");
      setCompanies([]);
    } finally {
      setLoading(false);
    }
  };

  // 🔥 LOGIN PAGE
  if (!isAuthenticated) {
    return (
      <div style={styles.loginWrapper}>
        <div style={styles.loginCard}>
          <h1 style={styles.logo}>CleanCompany</h1>
          <p style={styles.subtitle}>
            Secure company search powered by Auth0
          </p>

          <button
            style={styles.loginButton}
            onClick={() => loginWithRedirect()}
          >
            Sign in to continue
          </button>
        </div>
      </div>
    );
  }

  return (
    <div style={styles.container}>
      <div style={styles.header}>
        <h2>Company Lookup</h2>
        <div>
          <span style={{ marginRight: "15px" }}>
            {user?.name}
          </span>
          <button
            style={styles.logoutButton}
            onClick={() =>
              logout({ logoutParams: { returnTo: window.location.origin } })
            }
          >
            Logout
          </button>
        </div>
      </div>

      <div style={styles.searchBox}>
        <input
          style={styles.input}
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Search by name, org number, form, municipality..."
        />
        <button style={styles.searchButton} onClick={handleSearch}>
          Search
        </button>
      </div>

      {loading && <p>Loading...</p>}
      {error && <p style={styles.error}>{error}</p>}

      <div style={styles.results}>
        {companies.map((c) => (
          <div key={c.organizationNumber} style={styles.card}>
            <h3>{c.name}</h3>
            <p><strong>Org Number:</strong> {c.organizationNumber}</p>
            <p><strong>Form:</strong> {c.organizationForm}</p>
            <p><strong>Municipality:</strong> {c.municipality}</p>
          </div>
        ))}
      </div>
    </div>
  );
}

const styles: any = {
  loginWrapper: {
    height: "100vh",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    background: "linear-gradient(135deg, #1e3a8a, #2563eb)",
  },
  loginCard: {
    background: "white",
    padding: "50px",
    borderRadius: "12px",
    textAlign: "center",
    boxShadow: "0 10px 30px rgba(0,0,0,0.2)",
    width: "400px",
  },
  logo: {
    marginBottom: "10px",
    fontSize: "28px",
    fontWeight: "bold",
    color: "#1e3a8a",
  },
  subtitle: {
    marginBottom: "30px",
    color: "#555",
  },
  loginButton: {
    width: "100%",
    padding: "12px",
    backgroundColor: "#2563eb",
    color: "white",
    border: "none",
    borderRadius: "6px",
    fontSize: "16px",
    cursor: "pointer",
  },
  container: {
    maxWidth: "900px",
    margin: "40px auto",
    padding: "20px",
    fontFamily: "Arial, sans-serif",
  },
  header: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: "30px",
  },
  logoutButton: {
    padding: "8px 14px",
    border: "none",
    backgroundColor: "#e5e7eb",
    cursor: "pointer",
  },
  searchBox: {
    display: "flex",
    gap: "10px",
    marginBottom: "20px",
  },
  input: {
    flex: 1,
    padding: "10px",
    fontSize: "16px",
  },
  searchButton: {
    padding: "10px 20px",
    backgroundColor: "#2563eb",
    color: "white",
    border: "none",
    cursor: "pointer",
  },
  error: {
    color: "red",
  },
  results: {
    display: "grid",
    gap: "15px",
  },
  card: {
    padding: "15px",
    border: "1px solid #ddd",
    borderRadius: "8px",
    backgroundColor: "#f9fafb",
  },
};

export default App;
