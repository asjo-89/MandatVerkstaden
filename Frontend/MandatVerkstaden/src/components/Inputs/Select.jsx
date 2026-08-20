
export const Select = ({ htmlFor, label, value, onChange, defaultOptValue, options = [], width }) => {
  return (
    <>
        <div className={`input-group ${width}`}>
            <label 
                htmlFor={htmlFor} 
                className="manrope-semibold">
                    {label}
            </label>
            <select 
                id={htmlFor} 
                name={htmlFor} 
                value={value} 
                onChange={onChange}>
                    <option value="" disabled>
                        {defaultOptValue || "Välj"}
                    </option>
                    {options.map((option) => (
                        <option 
                            key={option.value} 
                            value={option.value}>
                                {option.label}
                        </option>
                    ))}
            </select>
        </div>
    </>
  )
}
