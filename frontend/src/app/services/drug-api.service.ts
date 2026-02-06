import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface PopularSearch {
  query: string;
  count: number;
}

@Injectable({
  providedIn: 'root',
})
export class DrugApiService {
  // backend base URL
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getPopularSearches(limit: number = 5): Observable<PopularSearch[]> {
    return this.http.get<PopularSearch[]>(
      `${this.baseUrl}/drugs/popular?limit=${limit}`,
    );
  }

  searchDrug(query: string, allergens?: string) {
    let url = `${this.baseUrl}/drugs/search?query=${encodeURIComponent(query)}`;
    if (allergens) {
      url += `&allergens=${encodeURIComponent(allergens)}`;
    }
    return this.http.get(url);
  }

  notifyAvailability(drugKey: string, email: string) {
    return this.http.post<any>(`${this.baseUrl}/availability-requests/notify`, {
      drugKey,
      email,
    });
  }

  getTopAdverseEvents(query: string, limit = 10) {
    return this.http.get<{
      drugKey: string;
      items: { term: string; count: number }[];
    }>(
      `${this.baseUrl}/drugs/top-adverse-events?query=${encodeURIComponent(query)}&limit=${limit}`,
    );
  }

  getSimilarDrugs(query: string, excludeDrugKey?: string, limit = 6) {
    let url = `${this.baseUrl}/drugs/similar?query=${encodeURIComponent(query)}&limit=${limit}`;
    if (excludeDrugKey)
      url += `&excludeDrugKey=${encodeURIComponent(excludeDrugKey)}`;
    return this.http.get<{
      query: string;
      ingredientKey: string | null;
      items: any[];
    }>(url);
  }

  getDrugUsage(query: string) {
    return this.http.get<{
      query: string;
      drugKey: string;
      usageText: string | null;
      hasUsageInfo: boolean;
    }>(`${this.baseUrl}/drugs/usage?query=${encodeURIComponent(query)}`);
  }
}
