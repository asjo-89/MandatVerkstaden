export const Input = ({ id, name, htmlFor, label, placeholder, type, onChange, statusClass, value, width }) => {
  return (
        <>
            <div className={`input-group ${width}`}>
                <label 
                    htmlFor={htmlFor} 
                    className="manrope-semibold">
                        {label}
                </label>
                <input 
                    className={statusClass} 
                    type={type || "text"} 
                    id={id} 
                    name={name} 
                    onChange={onChange} 
                    placeholder={placeholder} 
                    value={value} />
            </div>
        </>
    )
}
