
export const Select = ({ htmlFor, label, value, onChange, defaultOptValue, options = [] }) => {
  return (
    <>
        <div className="input-group input-group-medium">
            <label htmlFor={htmlFor} className="manrope-semibold">{label}</label>
            <select id={htmlFor} name={htmlFor} value={value} onChange={onChange}>
                <option value="">{defaultOptValue || "Välj"}</option>
                {options.map((option) => (
                    <option key={option.value} value={option.value}>{option.label}</option>
                ))}
            </select>
        </div>
    </>
  )
}
