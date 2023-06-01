import { createSlice, PayloadAction } from '@reduxjs/toolkit';

import { toDoApi } from '../../apis/ToDoApi';
import { DefaultPageInfo } from '../../constants';
import { ConvertToJSDateTime } from '../../helpers';
import { ToDoModel, ToDoQueryModel } from '../../models';
import type { RootState } from '../../redux/store';
import { ToDoListState } from './types';

const initialState: ToDoListState = {
  rows: [],
  isAdd: true,
  isLoading: false,
  addEditDialogOpen: false,
  query: {
    status: undefined,
    dueAtStart: undefined,
    dueAtEnd: undefined,
  },
  pageInfo: DefaultPageInfo,
  user: {
    user: 'User 1',
  },
};

export const toDoListSlice = createSlice({
  name: 'toDoList',
  initialState,
  reducers: {
    setRows(state, action: PayloadAction<ToDoModel[]>) {
      state.rows = action.payload;
    },
    setCurrentRow(state, action: PayloadAction<ToDoModel>) {
      state.currentRow = action.payload;
    },
    setQuery(state, action: PayloadAction<ToDoQueryModel>) {
      state.query = action.payload;
    },
    setAddEditDialogOpen(state, action: PayloadAction<boolean>) {
      state.addEditDialogOpen = action.payload;
    },
    setIsAdd(state, action: PayloadAction<boolean>) {
      state.isAdd = action.payload;
    },
    setIsLoading(state, action: PayloadAction<boolean>) {
      state.isLoading = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addMatcher(
        toDoApi.endpoints.getToDosQueryList.matchFulfilled,
        (state, { payload }) => {
          payload.data.forEach((toDo) => {
            if (toDo.dueAt) {
              toDo.dueAt = ConvertToJSDateTime(toDo.dueAt);
            }
            if (toDo.addAt) {
              toDo.addAt = ConvertToJSDateTime(toDo.addAt);
            }
            if (toDo.editAt) {
              toDo.editAt = ConvertToJSDateTime(toDo.editAt);
            }
          });
          state.rows = payload.data;
          state.pageInfo = payload.pageInfo;
          state.isLoading = false;
        },
      )
      .addMatcher(toDoApi.endpoints.getToDosQueryList.matchPending, (state) => {
        state.isLoading = true;
      })
      .addMatcher(
        toDoApi.endpoints.getToDosQueryList.matchRejected,
        (state) => {
          state.rows = [];
          state.pageInfo = DefaultPageInfo;
          state.isLoading = false;
        },
      )
      .addMatcher(toDoApi.endpoints.addToDo.matchFulfilled, (state) => {
        state.currentRow = undefined;
        state.addEditDialogOpen = false;
        state.isLoading = false;
      })
      .addMatcher(toDoApi.endpoints.addToDo.matchPending, (state) => {
        state.isLoading = true;
      })
      .addMatcher(toDoApi.endpoints.addToDo.matchRejected, (state) => {
        state.isLoading = false;
      })
      .addMatcher(toDoApi.endpoints.editToDo.matchFulfilled, (state) => {
        state.currentRow = undefined;
        state.addEditDialogOpen = false;
        state.isLoading = false;
      })
      .addMatcher(toDoApi.endpoints.editToDo.matchPending, (state) => {
        state.isLoading = true;
      })
      .addMatcher(toDoApi.endpoints.editToDo.matchRejected, (state) => {
        state.isLoading = false;
      })
      .addMatcher(toDoApi.endpoints.removeToDo.matchFulfilled, (state) => {
        state.isLoading = false;
      })
      .addMatcher(toDoApi.endpoints.removeToDo.matchPending, (state) => {
        state.isLoading = true;
      })
      .addMatcher(toDoApi.endpoints.removeToDo.matchRejected, (state) => {
        state.isLoading = false;
      });
  },
});

export const {
  setRows,
  setCurrentRow,
  setQuery,
  setAddEditDialogOpen,
  setIsAdd,
  setIsLoading,
} = toDoListSlice.actions;

export const toDoListSliceListSlice = (state: RootState) => state.toDoList;

export const toDoListReducer = toDoListSlice.reducer;
