const Input = ({ htmlFor, label, placeholder, type }) => {
  return (
        <>
            <div className="input-group input-group-medium">
                <label htmlFor={htmlFor} className="manrope-semibold">{label}</label>
                <input type={type || "text"} id={htmlFor} name={htmlFor} placeholder={placeholder} />
            </div>
        </>
    )
}

export default Input