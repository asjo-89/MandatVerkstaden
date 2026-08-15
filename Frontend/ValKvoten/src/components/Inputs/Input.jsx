const Input = ({ id, name, htmlFor, label, placeholder, type, onChange, statusClass }) => {
  return (
        <>
            <div className="input-group input-group-medium">
                <label htmlFor={htmlFor} className="manrope-semibold">{label}</label>
                <input className={statusClass} type={type || "text"} id={id} name={name} onChange={onChange} placeholder={placeholder} />
            </div>
        </>
    )
}

export default Input