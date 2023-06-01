import { useSnackbar } from 'notistack';
import { useTranslation } from 'react-i18next';

import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  SelectField,
  TextField,
} from '../../components';
import { DefaultPageInfo } from '../../constants';
import { DateStringFormat, NoEmptyPropertyValues } from '../../helpers';
import { ToDoModel } from '../../models';
import {
  useAddToDoMutation,
  useAppDispatch,
  useEditToDoMutation,
  useLazyGetToDosQueryListQuery,
  useStoreSelector,
} from '../../redux/store';
import { setCurrentRow } from './ToDoListSlice';
import { AddEditToDoDialogProps } from './types';

const statusOptions = [
  {
    value: '0',
    label: 'Not Started',
  },
  {
    value: '1',
    label: 'In Progress',
  },
  {
    value: '2',
    label: 'Completed',
  },
];

export function AddEditToDoDialog(props: AddEditToDoDialogProps) {
  const { t } = useTranslation();
  const { enqueueSnackbar } = useSnackbar();
  const { open, close } = props;
  const [addToDoAsync] = useAddToDoMutation();
  const [editToDoAsync] = useEditToDoMutation();
  const [getToDosAsync] = useLazyGetToDosQueryListQuery();
  const { currentRow, isAdd, user, query } = useStoreSelector(
    (state) => state.toDoList,
  );
  const dispatch = useAppDispatch();

  const updateValue = <K extends keyof ToDoModel>(
    key: K,
    value: ToDoModel[K],
  ) => {
    const updatedToDo: ToDoModel = JSON.parse(JSON.stringify(currentRow));
    if (updatedToDo![key] !== value) {
      updatedToDo![key] = value;
      dispatch(setCurrentRow(updatedToDo));
    }
  };

  const onSubmit = async () => {
    try {
      if (NoEmptyPropertyValues(currentRow!)) {
        if (isAdd) {
          await addToDoAsync({ data: currentRow, params: user }).unwrap();
        } else {
          await editToDoAsync({
            id: currentRow!.id!.toString(),
            data: currentRow,
            params: user,
          }).unwrap();
        }
        await getToDosAsync({
          params: {
            pageNumber: DefaultPageInfo.pageNumber,
            itemsPerPage: DefaultPageInfo.itemsPerPage,
            ...query,
          },
        });
        enqueueSnackbar(t('common.success'), {
          variant: 'success',
          autoHideDuration: 3000,
        });
      } else {
        enqueueSnackbar(t('common.invalidInput'), {
          variant: 'error',
          autoHideDuration: 3000,
        });
      }
    } catch {
      enqueueSnackbar(t('common.error'), {
        variant: 'error',
        autoHideDuration: 3000,
      });
    }
  };

  return (
    <Dialog open={open} handleClose={close}>
      <DialogTitle>{isAdd ? t('toDo.add') : t('toDo.edit')}</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          id='name'
          label={t('toDo.name')!}
          fullWidth
          variant='standard'
          defaultValue={currentRow!.name}
          onBlur={(e) => updateValue('name', e.target.value!)}
        />
        <TextField
          autoFocus
          id='description'
          label={t('toDo.description')!}
          fullWidth
          variant='standard'
          defaultValue={currentRow!.description}
          onBlur={(e) => updateValue('description', e.target.value!)}
        />
        <TextField
          autoFocus
          id='dueAt'
          label={t('toDo.dueAt')!}
          type='date'
          fullWidth
          variant='standard'
          defaultValue={DateStringFormat(currentRow!.dueAt!)}
          onBlur={(e) => updateValue('dueAt', new Date(e.target.value!))}
        />
        <SelectField
          autoFocus
          id='status'
          label={t('toDo.status')!}
          fullWidth
          variant='standard'
          select
          value={currentRow!.status}
          onChange={(e) => {
            updateValue('status', parseInt(e.target.value!));
          }}
          options={statusOptions}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={() => close()}>{t('toDo.cancelEdit')}</Button>
        <Button onClick={() => onSubmit()}>
          {isAdd ? t('toDo.add') : t('toDo.edit')}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
