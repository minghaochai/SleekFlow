import { ToDoModel } from '../models';
import { BaseActionApi } from './BaseActionApi';
import { emptySplitApi } from './EmptySplitApi';

const baseActions = new BaseActionApi<ToDoModel>('/todos');
const toDoApi = emptySplitApi.injectEndpoints({
  endpoints(builder) {
    return {
      getToDoById: baseActions.getById(builder),
      getToDosQueryList: baseActions.getQueryList(builder),
      addToDo: baseActions.add(builder),
      editToDo: baseActions.edit(builder),
      removeToDo: baseActions.delete(builder),
    };
  },
  overrideExisting: false,
});

export const {
  useLazyGetToDosQueryListQuery,
  useAddToDoMutation,
  useEditToDoMutation,
  useRemoveToDoMutation,
} = toDoApi;
export { toDoApi };
