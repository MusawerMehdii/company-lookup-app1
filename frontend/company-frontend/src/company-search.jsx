import { useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";

const API_BASE = "https://localhost:7242";

function CompanySearch() {
  const { getAccessTokenSilently } = useAuth0();

  const [query, setQuery] = useState("");
  const [companies, setCompanies] = useState([]); // ✅ FIX IS HERE
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const search = async () => {
    if (!query) {
      setError("Please enter a search term");
      return;
    }

    try {
      setLoading(true);
      setError(null);

      const token = await getAccessTokenSilently();

      const response = await axios.get(
        `${API_BASE}/api/companies?query=${query}`,
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );

      setCompanies(response.data);
    } catch (err) {
      setError("Failed to fetch companies");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card">
      <h2>Search Companies</h2>

      <div className="search-row">
        <input
          className="input"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Enter company name"
        />
        <button className="btn" onClick={search}>
          Search
        </button>
      </div>

      {loading && <p>Loading…</p>}
      {error && <p className="error">{error}</p>}

      <ul className="results">
        {companies.map((c) => (
          <ul className="results">
  {companies.map((c) => (
    <li key={c.organizationNumber} className="company-item">
      <div className="company-name">{c.name}</div>

      <div className="company-meta">
        <div>
          <strong>Org.nr:</strong> {c.organizationNumber}
        </div>

        <div>
          <strong>Organization form:</strong> {c.organizationForm}
        </div>

        <div>
          <strong>Municipality:</strong> {c.municipality}
        </div>
      </div>
    </li>
  ))}
</ul>

        ))}
      </ul>
    </div>
  );
}

export default CompanySearch;
