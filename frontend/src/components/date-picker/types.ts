import { DateValidationError } from '@mui/x-date-pickers';
import { PickerChangeHandlerContext } from '@mui/x-date-pickers/internals/hooks/usePicker/usePickerValue.types';

export interface DatePickerProps {
  label: string;
  onChange: (
    value: Date | null,
    context: PickerChangeHandlerContext<DateValidationError>,
  ) => void;
}
