using System;

namespace Domain.IRepositories.Base;

public interface IBaseRepository<T> : ICrudRepository<T>, ISpetificationRepository<T>
    where T : class;