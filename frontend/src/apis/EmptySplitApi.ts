import { createApi } from '@reduxjs/toolkit/query/react';

import { baseQuery } from '../helpers';

export const emptySplitApi = createApi({
  reducerPath: 'api',
  baseQuery: baseQuery,
  endpoints: () => ({}),
});
