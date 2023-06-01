import { TextField as MuiTextField } from '@mui/material';
import { unstable_extendSxProp as extendSxProp } from '@mui/system';

import { TextFieldProps } from './types';

export function TextField(props: TextFieldProps) {
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
    onBlur,
    onChange,
  } = props;
  const { sx: customXs } = extendSxProp({ sx });

  return (
    <MuiTextField
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
      {select ? props.children : null}
    </MuiTextField>
  );
}

export default TextField;
