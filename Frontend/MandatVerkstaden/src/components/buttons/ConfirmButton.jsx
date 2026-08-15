
export const ConfirmButton = ({ btnText, onClick, className, btnType }) => {
  return (
        <>
            <button type={btnType || "button"} className={`btn btn-confirm manrope-semibold ${className || ""}`} onClick={onClick}>{btnText || ""}</button>
        </>
  )
}
