type ValidationTypes =
  | 'required'
  | 'email'
  | `maxlength:${number}`
  | `minlength:${number}`
  | `customregex:${string}`;

export type PickNullable<T> = {
  [P in keyof T as null extends T[P] ? P : never]: T[P];
};

export type Rules<T> = { [key in keyof T]?: ValidationTypes[] };

export type InputErrorType<T> = { [K in PickNullable<keyof T>]?: boolean };

export const customInputValidation = <T>(
  key: keyof T,
  value: any,
  rules: Rules<T>,
  errors: { [K in PickNullable<keyof T>]?: boolean },
) => {
  const updatedErrors: { [K in PickNullable<keyof T>]?: boolean } = JSON.parse(
    JSON.stringify(errors),
  );
  updatedErrors[key] = false;
  if (rules[key] !== undefined) {
    //Required
    if (
      rules[key]!.includes('required') &&
      (value === undefined ||
        value === null ||
        (typeof value === 'string' && value === '') ||
        (typeof value === 'string' && value.replace(/\s/g, '').length === 0))
    ) {
      updatedErrors[key] = true;
    }
    //Email
    //Based email regex on https://www.w3resource.com/javascript/form/email-validation.php
    if (
      rules[key]!.includes('email') &&
      (typeof value !== 'string' ||
        !value.match(
          "^[\\w-\\.!#$%&'*+\\-=/?^`{}|~]+@([\\w-]+\\.)+[\\w-]{2,4}$",
        ))
    ) {
      updatedErrors[key] = true;
    }
    //Max Length
    const maxLengthRegex = new RegExp('(maxlength:)[0-9]+$');
    const maxLengthRule = rules[key]!.find((x) => maxLengthRegex.test(x));
    if (maxLengthRule !== undefined) {
      const maxLength = parseInt(maxLengthRule.split(':')[1]);
      if (
        typeof value !== 'string' ||
        (value.length !== 0 && value.length > maxLength)
      ) {
        updatedErrors[key] = true;
      }
    }
    //Min length
    const minLengthRegex = new RegExp('(minlength:)[0-9]+$');
    const minLengthRule = rules[key]!.find((x) => minLengthRegex.test(x));
    if (minLengthRule !== undefined) {
      const minLength = parseInt(minLengthRule.split(':')[1]);
      if (
        typeof value !== 'string' ||
        (value.length !== 0 && value.length < minLength)
      ) {
        updatedErrors[key] = true;
      }
    }
    //Custom regex
    const customRegexRegex = new RegExp('(customregex:).*');
    const customRegexRule = rules[key]!.find((x) => customRegexRegex.test(x));
    if (customRegexRule !== undefined) {
      const customRegex = new RegExp(customRegexRule.split(':')[1]);
      if (typeof value !== 'string' || !customRegex.test(value)) {
        updatedErrors[key] = true;
      }
    }
  }
  //No validation errors
  if (updatedErrors[key] === false) {
    delete updatedErrors[key];
  }
  return updatedErrors;
};
export const customValidateAll = <T>(
  object: T,
  rules: Rules<T>,
  errors: { [K in PickNullable<keyof T>]?: boolean },
) => {
  let updatedErrors: { [K in PickNullable<keyof T>]?: boolean } = JSON.parse(
    JSON.stringify(errors),
  );
  for (const property in rules) {
    updatedErrors = customInputValidation(
      property,
      object[property],
      rules,
      errors,
    );
  }
  return updatedErrors;
};
