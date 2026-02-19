import { useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";
import { Company } from "./types";

const API_BASE = "https://localhost:7242";

function CompanySearch() {
  const { getAccessTokenSilently } = useAuth0();

  const [query, setQuery] = useState<string>("");
  const [companies, setCompanies] = useState<Company[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const search = async () => {
    const trimmed = query.trim();

    if (!trimmed) {
      setError("Please enter a company name or organization number.");
      return;
    }

    try {
      setLoading(true);
      setError(null);
      setCompanies([]);

      const token = await getAccessTokenSilently({
        authorizationParams: {
          audience: "https://company-api"
        }
      });

      // Detect if input is organization number (only digits)
      const isOrgNumber = /^\d+$/.test(trimmed);

      if (isOrgNumber) {
        // 🔥 Search by organization number
        const response = await axios.get<Company>(
          `${API_BASE}/api/companies/${trimmed}`,
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );

        setCompanies([response.data]);
      } else {
        // 🔥 Search by name
        const response = await axios.get<Company[]>(
          `${API_BASE}/api/companies?query=${trimmed}`,
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );

        if (response.data.length === 0) {
          setError("No companies found.");
        }

        setCompanies(response.data);
      }
    } catch (err: any) {
      if (err.response?.status === 404) {
        setError("Company not found.");
      } else if (err.response?.status === 401) {
        setError("Unauthorized. Please login again.");
      } else {
        setError("Something went wrong while fetching companies.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card">
      <h2 className="title">Company Lookup</h2>

      <div className="search-row">
        <input
          className="input"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Search by name or organization number"
          onKeyDown={(e) => {
            if (e.key === "Enter") search();
          }}
        />
        <button className="btn" onClick={search}>
          {loading ? "Searching..." : "Search"}
        </button>
      </div>

      {error && <p className="error">{error}</p>}

      <div className="results-grid">
        {companies.map((c) => (
          <div key={c.organizationNumber} className="company-card">
            <div className="company-name">{c.name}</div>

            <div className="company-meta">
              <div>
                <strong>Org.nr:</strong> {c.organizationNumber}
              </div>

              <div>
                <strong>Form:</strong> {c.organizationForm}
              </div>

              <div>
                <strong>Municipality:</strong> {c.municipality}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default CompanySearch;
