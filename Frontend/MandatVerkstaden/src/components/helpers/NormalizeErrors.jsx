
// const NormalizeErrors = () => {
  const NormalizeErrors = (errors) => {
    console.log("Normalizing errors:", errors);
    const normalizedErrors = {};
    if(errors && typeof errors === "object") {
      Object.entries(errors).forEach(([key, message]) => {
        normalizedErrors[key.toLowerCase()] = Array.isArray(message) ? message.join(", \n") : message;
      })
    }
    return normalizedErrors;
  }

//   return normalizeErrors;
// }

export default NormalizeErrors