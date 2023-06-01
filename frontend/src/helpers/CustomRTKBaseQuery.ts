import { fetchBaseQuery } from '@reduxjs/toolkit/dist/query/react';

export const baseQuery = fetchBaseQuery({
  baseUrl: process.env.REACT_APP_API_BASE_URL,
  timeout: 30000,
  prepareHeaders: (headers) => {
    return headers;
  },
});
