import { MenuItem } from '@mui/material';
import { unstable_extendSxProp as extendSxProp } from '@mui/system';

import TextField from './TextField';
import { SelectFieldProps } from './types';

export function SelectField(props: SelectFieldProps) {
  const {
    autoFocus,
    id,
    label,
    type,
    fullWidth,
    variant,
    select,
    defaultValue,
    value,
    sx,
    options,
    onBlur,
    onChange,
  } = props;
  const { sx: customXs } = extendSxProp({ sx });

  return (
    <TextField
      autoFocus={autoFocus}
      id={id}
      label={label}
      type={type}
      fullWidth={fullWidth}
      variant={variant}
      defaultValue={defaultValue}
      select={select !== undefined ? select : undefined}
      value={value}
      onBlur={onBlur}
      onChange={onChange}
      sx={customXs}
    >
      {options.map((option) => (
        <MenuItem key={option.value} value={option.value}>
          {option.label}
        </MenuItem>
      ))}
    </TextField>
  );
}
