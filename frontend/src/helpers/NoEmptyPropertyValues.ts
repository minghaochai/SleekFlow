export function NoEmptyPropertyValues(object: object) {
  for (const [_key, value] of Object.entries(object)) {
    if (value === '') {
      return false;
    }
  }
  return true;
}
