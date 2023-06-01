import { useTranslation } from 'react-i18next';

import { Button, DatePicker, SelectField } from '../../components';
import { DefaultPageInfo } from '../../constants';
import { StatusType } from '../../enums/StatusType';
import { ToDoQueryModel } from '../../models';
import {
  useAppDispatch,
  useLazyGetToDosQueryListQuery,
  useStoreSelector,
} from '../../redux/store';
import { setQuery } from './ToDoListSlice';

export function ToDoFilter() {
  const { t } = useTranslation();
  const dispatch = useAppDispatch();
  const { query } = useStoreSelector((state) => state.toDoList);
  const [getToDosAsync] = useLazyGetToDosQueryListQuery();

  const statusOptions: { value: string | number; label: string }[] = [
    {
      value: '-',
      label: '-',
    },
    {
      value: 0,
      label: 'Not Started',
    },
    {
      value: 1,
      label: 'In Progress',
    },
    {
      value: 2,
      label: 'Completed',
    },
  ];

  const updateFilter = <K extends keyof ToDoQueryModel>(
    key: K,
    value: ToDoQueryModel[K],
  ) => {
    const updatedQuery: ToDoQueryModel = JSON.parse(JSON.stringify(query));
    if (updatedQuery![key] !== value) {
      updatedQuery![key] = value;
      dispatch(setQuery(updatedQuery));
    }
  };

  const onApplyFilter = () => {
    getToDosAsync({
      params: {
        pageNumber: DefaultPageInfo.pageNumber,
        itemsPerPage: DefaultPageInfo.itemsPerPage,
        ...query,
      },
    });
  };

  return (
    <>
      <SelectField
        autoFocus
        id='status-filter'
        label={t('toDo.status')!}
        variant='outlined'
        select
        defaultValue={'-'}
        sx={{ marginTop: '8px', width: '250px' }}
        onChange={(e) => {
          const value = e.target.value!;
          const numValue = parseInt(value, 10);
          let statusType = undefined;
          if (numValue === 0) {
            statusType = StatusType.NotStarted;
          } else if (numValue === 1) {
            statusType = StatusType.InProgress;
          } else if (numValue === 2) {
            statusType = StatusType.Completed;
          } else {
            statusType = undefined;
          }
          updateFilter('status', statusType);
        }}
        options={statusOptions}
      />

      <DatePicker
        label={t('toDo.dueAtFrom')!}
        onChange={(e) => {
          updateFilter('dueAtStart', e ?? undefined);
        }}
      />
      <DatePicker
        label={t('toDo.dueAtBefore')!}
        onChange={(e) => {
          updateFilter('dueAtEnd', e ?? undefined);
        }}
      />
      <Button onClick={() => onApplyFilter()}>{t('toDo.applyFilter')}</Button>
    </>
  );
}
