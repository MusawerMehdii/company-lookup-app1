const API_BASE = "https://localhost:7242";

export async function searchCompanies(query: string, token: string) {
  const response = await fetch(
    `${API_BASE}/api/companies?query=${encodeURIComponent(query)}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    throw new Error(`Error ${response.status}`);
  }

  return response.json();
}

export async function getCompany(orgnr: string, token: string) {
  const response = await fetch(
    `${API_BASE}/api/companies/${orgnr}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    throw new Error(`Error ${response.status}`);
  }

  return response.json();
}
