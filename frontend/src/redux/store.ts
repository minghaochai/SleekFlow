import { Action, configureStore, ThunkAction } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import { emptySplitApi } from '../apis/EmptySplitApi';
import { toDoListReducer } from '../features/todo/ToDoListSlice';
import { rtkQueryErrorLogger } from '../helpers';

export const store = configureStore({
  reducer: {
    toDoList: toDoListReducer,
    [emptySplitApi.reducerPath]: emptySplitApi.reducer,
  },
  middleware: (getDefaultMiddleware) => {
    return getDefaultMiddleware({
      serializableCheck: false,
    })
      .concat(rtkQueryErrorLogger)
      .concat(emptySplitApi.middleware);
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useStoreSelector: TypedUseSelectorHook<RootState> = useSelector;
export {
  useAddToDoMutation,
  useEditToDoMutation,
  useLazyGetToDosQueryListQuery,
  useRemoveToDoMutation,
} from '../apis/ToDoApi';
