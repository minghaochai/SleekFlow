import 'dayjs/locale/en-gb';

import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DatePicker as MuiDatePicker } from '@mui/x-date-pickers/DatePicker';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';

import { DatePickerProps } from './types';

export function DatePicker(props: DatePickerProps) {
  const { label, onChange } = props;

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale='en-gb'>
      <DemoContainer components={['DatePicker']}>
        <MuiDatePicker label={label} onChange={onChange} />
      </DemoContainer>
    </LocalizationProvider>
  );
}

export default DatePicker;
